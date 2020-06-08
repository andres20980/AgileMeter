import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/UserService';
import { AppComponent } from '../app.component';
import { Router } from "@angular/router";
import { map } from "rxjs/operators";
import { environment } from "../../environments/environment.prod"

export interface UserPermission {
  NombreCompleto: string,
  Password: string,
  //{NombreCompleto:"admin",Password:"Admin" , Role: {id:2,role:"Administrador"}, Activo:true,IdiomaFavorito:"en",nombre:"Admin"}
}


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  providers: [UserService]
})


export class LoginComponent implements OnInit {

  public ErrorMessage: string = null;
  public nombreDeUsuario: string = "";
  public passwordDeUsuario: string = "";
  public Recuerdame: boolean = false;
  public enviando: boolean = false;
  public date = new Date();
  public user: UserPermission;
  public loginEnabled: boolean = environment.loginEnabled;

  constructor(private _userService: UserService,
    private _router: Router,
    private _app: AppComponent) { }


  ngOnInit() {
    //Comprobamos si anteriormente queria que se recordara su usuario
    if (this.recuerdameOnOff()) {
      this.nombreDeUsuario = localStorage.getItem("user");
      this.passwordDeUsuario = localStorage.getItem("passuser");
      this.Recuerdame = true;
    }
  }

  public TrySignUp(event){
    if(event.keyCode == 13) {
      this.SignUp();
    }
  }

  //Mediante este metodo comprobaremos si el usuario espcificado existe o no
  //si es así sera redirigido a la pagina principal
  public SignUp() {
    this.ErrorMessage = null;
    this.enviando = true;

    //Comprueba si los campos tienen datos
    if (this.nombreDeUsuario != "" && this.passwordDeUsuario != "") {
      //Manda una petición a la api para ver si el nombre existe
      var User = { 'nombre': this.nombreDeUsuario, 'password': this.passwordDeUsuario };
      this._userService.SignUpMe(User).subscribe(
        res => {
          
          //Si no existe muestra un error
          if (!res) {
            this.ErrorMessage = "No existe el usuario especificado"
          } else {
            //Si existe comprueba si la contraseña es correcta y es redirigido
            if (this.Recuerdame) {
              //Si el usuario quiere ser recordado lo guardara en el localStorage
           
              localStorage.setItem("user", this.nombreDeUsuario);
              localStorage.setItem("passuser", this.passwordDeUsuario);
              localStorage.setItem("tokenuser", res.access_token);
              localStorage.setItem("userlongname", res.user_long_name);
            } else {
              //Si el usuario no quiere ser recordado lo guardara en el servicio
              this._app._storageDataService.UserData = { 'nombre': this.nombreDeUsuario};
              //Guardamos el token del usuario
              this._app._storageDataService.TokenUser = res.access_token;
              this._app._storageDataService.UserLongName = res.user_long_name;
            }
            this._router.navigate(['/home']);
          }

          this.getFavoriteLang(this.nombreDeUsuario);
        },
        error => {
          //Si el servidor tiene algún tipo de problema mostraremos este error
          if (error == 401) {
            this.ErrorMessage = "El nombre de usuario o contraseña no son correctos";
            this.passwordDeUsuario = "";
          } else if (error == 500) {
            this.ErrorMessage = "Ocurrio un error en el servidor";
          } else {
            this.ErrorMessage = "Ocurrió un error al conectar con el servidor";
          }
          this.enviando = false;

        }
      );
    } else {
      this.ErrorMessage = "Introduzca todos los campos"
      this.enviando = false;
    }
  }

  //Este metodo devuelve si el usuario estaba siendo recordado
  //Lo utilizamos en el html para cambiar el recuerdame y en el init del componente
  public recuerdameOnOff() {
    if (localStorage.getItem("user") != null && localStorage.getItem("user") != undefined) {
      return true;
    } else {
      return false;
    }
  }

  //Este metodo nos permite saber si el usuario quiere ser recordado
  public recuerdameChange() {
    if (!this.Recuerdame) {
      localStorage.removeItem("user");
      localStorage.removeItem("passuser");
      //this.Recuerdame = false;
    } else {
      //this.Recuerdame = true;
    }
  }

  public getFavoriteLang(userName: string)
  {
    this._userService.getUser(userName).pipe(map(x => x)).subscribe( r => {
      let currentUser: UserPermission = {NombreCompleto: r.nombreCompleto, Password: r.password} 
      this._app.ChangeLang(r.idiomaFavorito,currentUser)
    });
  }

}
