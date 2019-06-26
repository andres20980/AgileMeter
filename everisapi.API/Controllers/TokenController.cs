using everisapi.API.Models;
using everisapi.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Novell.Directory.Ldap;


namespace everisapi.API.Controllers
{

    [Route("api/Token")]
    public class TokenController : Controller
    {

        private IConfiguration Configuration;
        private IUsersInfoRepository _usersInfoRespository;

        public const String ldapHost="10.125.8.21";

        public TokenController(IConfiguration config, IUsersInfoRepository usersInfoRespository)
        {
            Configuration = config;
            _usersInfoRespository = usersInfoRespository;
        }

        [HttpPost()]
        public IActionResult CreateToken([FromBody] UsersSinProyectosDto UserAuth)
        {
            //In real example use LoginModel, this is just for dummy purpose so that
            //we can focus on relevant code            

            //Comprueba que el body del json es correcto sino devolvera null
            //Si esto ocurre devolveremos un error
            if (UserAuth == null)
            {
                return BadRequest();
            }

            //Si no cumple con el modelo de creación devuelve error
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Encriptamos la contraseña
             using (var sha256 = SHA256.Create())
             {
                 // Le damos la contraseña
                 var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(UserAuth.Password));
                 // Recogemos el hash como string
                 var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                 // Y se lo damos 
                 UserAuth.Password = hash;
             }

            IActionResult response = Unauthorized();
            //string userNombreLargo = IsUserExistsLDAP(UserAuth.Nombre, UserAuth.Password);
            /*string userNombreLargo = UserAuth.Nombre;
            if (userNombreLargo != null && userNombreLargo != "")
            {
                //Check customer if exists in our database
                if (_usersInfoRespository.UserAuth(UserAuth))
                {
                    //create jwt token here and send it with response
                    var jwtToken = JwtTokenBuilder();
                    response = Ok(new { access_token = jwtToken, user_long_name = userNombreLargo });
                }else{
                    //Is a new user
                    this.isNewUser(userNombreLargo, UserAuth.Nombre);
                    var jwtToken = JwtTokenBuilder();
                    response = Ok(new { access_token = jwtToken, user_long_name = userNombreLargo});
                  
                }
            }*/
             if (UserAuth.Nombre != null && UserAuth.Nombre != "")
            { //Check customer if exists in our database
                if (_usersInfoRespository.UserAuth(UserAuth))
                {
                    string userNombreLargo = _usersInfoRespository.getNombreCompleto(UserAuth.Nombre);
                    string[] ss = userNombreLargo.Split(' ');
                    string s2 = ss[0];
                    if(ss.Length > 1){
                        s2 += " " + ss[1];
                    }
                    userNombreLargo = s2;
                    //create jwt token here and send it with response
                    var jwtToken = JwtTokenBuilder();
                    response = Ok(new { access_token = jwtToken, user_long_name = userNombreLargo });
                }
            }
            return response;
        }

        private void isNewUser(string userNombreLargo, string nombre)
        {
            Entities.UserEntity newUser = new Entities.UserEntity();
                  newUser.Nombre = nombre;
                  newUser.Password = "default-password";
                  newUser.RoleId = (int)Roles.User;
                  _usersInfoRespository.AddUser(newUser);
                  _usersInfoRespository.AddProjectTest(newUser.Nombre);
        }

        private string IsUserExistsLDAP(string name, string pwd)
        {
            // Metemos los valores de configuración para conectarnos al ldap de Everis.
            int LdapPort = LdapConnection.DEFAULT_PORT;
            //int searchScope = LdapConnection.SCOPE_ONE;
            int LdapVersion = LdapConnection.Ldap_V3;
            //bool attributeOnly=true;
            String[] attrs = { LdapConnection.NO_ATTRS };
            LdapConnection lc = new LdapConnection();
            string resultado = "";
            // Vamos a meter una restricción de tiempo.
            LdapSearchConstraints constraints = new LdapSearchConstraints();
            constraints.TimeLimit = 10000; // ms
           
            try{
                // Nos conectamos al servidor.
                lc.Connect(ldapHost,LdapPort);
                // Accedemos con las credenciales del usuario para ver si está.
                lc.Bind(LdapVersion, Configuration["connectionStrings:LDAPDomain"] + name, pwd);
                
                // Set values to search
                string base1="OU=Spain,OU=Europe,OU=Everis,DC=usersad,DC=everis,DC=int";
                string[] attributes = new string[] {"displayName","samaccountname"};
                string filter=String.Format("(&(objectClass=user)(samaccountname={0}))", name);
                LdapSearchQueue lsc=lc.Search(base1,LdapConnection.SCOPE_SUB,filter,attributes,false,(LdapSearchQueue)null,(LdapSearchConstraints)null);
                LdapMessage msg;
                if((msg = lsc.getResponse()) != null) {
                    if(msg is LdapSearchResult) {
                         LdapEntry nextEntry = ((LdapSearchResult)msg).Entry;
                         LdapAttributeSet attributeSet = nextEntry.getAttributeSet();
                         //Console.WriteLine("Nombre corto: "+attributeSet.getAttribute("samaccountname").StringValue);
                         //Console.WriteLine("Nombre Largo: "+attributeSet.getAttribute("displayName").StringValue);
                         string[] ss = attributeSet.getAttribute("displayName").StringValue.Split(' ');
                         string s2 = ss[0];
                         if(ss.Length > 1){
                             s2 += " " + ss[1];
                         }
                         return s2;
                    }
                }
                 
                lc.Disconnect();
           } catch (LdapException e) {
                   Console.WriteLine(e.Message);
                   return null;
           } catch (Exception) { 
               Console.WriteLine("error");
               return null;
           }
           return resultado;
        }

        //Metodo para crear tokens
        private object JwtTokenBuilder()
        {
            //Preparamos la clave y las credenciales
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:key"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(issuer: Configuration["JWT:issuer"],
              audience: Configuration["JWT:audience"], signingCredentials: credentials,
              expires: DateTime.Now.AddYears(1));

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
