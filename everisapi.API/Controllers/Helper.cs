using System;
using System.Text;
using System.Security.Cryptography;
using MimeKit;
using MimeKit.Utils;
using System.IO;


public static class Helper
{

    public static void emailBuilder(MimeMessage message, String user, String pass, String nombreCompleto, string idiomaFavorito)
    {

        var builder = new BodyBuilder();
        var image = builder.LinkedResources.Add(@"agilemeter_email.png");
        image.ContentId = MimeUtils.GenerateMessageId();

        if(idiomaFavorito.Equals("es")){
        // Set the html version of the message text
                message.Subject = "Bienvenido/a a AgileMeter";
                builder.HtmlBody = string.Format(@"
                    <div style='padding-bottom:20px;font-family: Calibri,Candara,Segoe,Segoe UI,Optima,Arial,sans-serif;font-size:18px;width: 620px;margin: 0 auto;'>
                    <center><a style='margin-right:10px;margin-top:2px' href='https://agilemeter.everis.com/'><img alt='AgileMeter' src=""cid:{0}""></a></center>
                        <br>
            <center><strong>¡Bienvenido/a {3} a AgileMeter!</strong></center>
            <br>
            <p style='padding-left:20px;padding-right:20px'>Estamos encantados de que formes parte de nuestra familia y de que te
                podamos ayudar a evaluar el nivel de madurez en Agile de tus equipos.
                Para comenzar, te facilitamos las credenciales que tendrás que usar para acceder a nuestra plataforma:
            </p>
            <div style='padding-left:20px;padding-right:20px'>
                <ul>
                    <li>URL: <a href='https://agilemeter.everis.com/'>https://agilemeter.everis.com/</a></li>
                    <li>Usuario: {1}</li>
                    <li>Contraseña: {2}</li>
                </ul>
            </div>
            <br>
            <hr>
            <br>
            <strong style='padding-left:20px;padding-right:20px'>¿Tienes dudas? ¿Te podemos ayudar?</strong>

            <p style='padding-left:20px;padding-right:20px'>Escribenos a agile.meter@everis.com, estaremos encantados de
                ayudarte.</p><br>
            <center>¡Qué tengas un día maravilloso!</center>

        </div> ", image.ContentId, user, pass, nombreCompleto);
        }
        else{
            // Set the html version of the message text
                message.Subject = "Welcome to Agilemeter!";
                builder.HtmlBody = string.Format(@"
                    <div style='padding-bottom:20px;font-family: Calibri,Candara,Segoe,Segoe UI,Optima,Arial,sans-serif;font-size:18px;width: 620px;margin: 0 auto;'>
                    <center><a style='margin-right:10px;margin-top:2px' href='https://agilemeter.everis.com/'><img alt='AgileMeter' src=""cid:{0}""></a></center>
                        <br>
            <center><strong>Welcome {3} to AgileMeter!</strong></center>
            <br>
            <p style='padding-left:20px;padding-right:20px'>
            We are delighted that you are part of our family and that we can help you assess the maturity level in Agile of your teams.
                To start, we provide you with the credentials that you will have to use to access our platform:
            </p>
            <div style='padding-left:20px;padding-right:20px'>
                <ul>
                    <li>URL: <a href='https://agilemeter.everis.com/'>https://agilemeter.everis.com/</a></li>
                    <li>User: {1}</li>
                    <li>Password: {2}</li>
                </ul>
            </div>
            <br>
            <hr>
            <br>
            <strong style='padding-left:20px;padding-right:20px'>Do you have any doubts? Can we help you?</strong>

            <p style='padding-left:20px;padding-right:20px'>Send us an email to agile.meter@everis.com, we will love to help you.</p><br>
            <center>Have a wonderful day!</center>

        </div> ", image.ContentId, user, pass, nombreCompleto);
        }

        // Now we just need to set the message body and we're done
        message.Body = builder.ToMessageBody();
    }


    public static string EncryptString(string key, string plainText)
    {
        byte[] iv = new byte[16];
        byte[] array;

        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }

                    array = memoryStream.ToArray();
                }
            }
        }

        return Convert.ToBase64String(array);
    }

    public static string DecryptString(string key, string cipherText)
    {
        byte[] iv = new byte[16];
        byte[] buffer = Convert.FromBase64String(cipherText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new MemoryStream(buffer))
            {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }
    }
}