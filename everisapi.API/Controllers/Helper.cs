using System;
using System.Text;
using System.Security.Cryptography;
using MimeKit;
using MimeKit.Utils;
using System.IO;


public static class Helper
{

    public static void emailBuilder(MimeMessage message, String user, String pass)
    {

        var builder = new BodyBuilder();
        var image = builder.LinkedResources.Add(@"..\front\src\assets\agilemeter_email.png");
        image.ContentId = MimeUtils.GenerateMessageId();

        // Set the html version of the message text
        builder.HtmlBody = string.Format(@"
            <div style='padding:20px; font-family: Calibri,Candara,Segoe,Segoe UI,Optima,Arial,sans-serif;font-size:18px;'>
            <center><a href='https://agilemeter.everis.com/'><img width='65%' height='50%' alt='AgileMeter' src=""cid:{0}""></a></center>
            <br>
            <hr>
            <br>
            <center><strong>¡Bienvenido/a a AgileMeter!</strong></center>
            <br>
            <p>Estamos encantados de que formes parte de nuestra familia y que te podamos ayudar a evaluar el nivel de madurez en Agile de tus equipos.
            Para comenzar, te facilitamos las credenciales que tendrás que usar para acceder a nuestra plataforma:
            </p>
            <ul>
                <li>Usuario: {1}</li>
                <li>Contraseña: {2}</li>
            </ul> 
            <br>
            <hr>
            <br>
            <strong>¿Tienes dudas? ¿Te podemos ayudar?</strong>
            
            <p>Escribenos a agile.meter@everis.com, estaremos encantados de ayudarte.</p><br>
            <center>¡Qué tengas un día maravilloso!</center>

            </div>", image.ContentId, user, pass);



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