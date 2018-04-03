import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/UserService';
import { AppComponent } from '../app.component'
import { User } from './User';
import {Router} from "@angular/router";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  providers: [UserService]
})
export class LoginComponent implements OnInit {

    public nombreDeUsuario: string="";
    public passwordDeUsuario: string="";
    public ErrorMessage: string = null;
    private UsuarioEntrante: User = null;

    constructor(private _userService : UserService,
        private _router: Router,
        private _app: AppComponent) { }


    ngOnInit() {
    }

    //Mediante este metodo comprobaremos si el usuario espcificado existe o no
    //si es así sera redirigido a la pagina principal
    public SignUp(){
        //Comprueba si los campos tienen datos
        if(this.nombreDeUsuario!="" && this.passwordDeUsuario!=""){
            //Manda una petición a la api para ver si el nombre existe
            this._userService.SignUpMe(this.nombreDeUsuario).subscribe(
                res => {
                    //Si no existe muestra un error
                    if(!res){
                        this.ErrorMessage = "No existe el usuario especificado."
                    }else{
                        //Si existe comprueba si la contraseña es correcta y es redirigido
                        this.UsuarioEntrante = res;
                        if(this.UsuarioEntrante.password==this.passwordDeUsuario){
                            this._app._storageDataService.UserData = this.UsuarioEntrante;
                            this._router.navigate(['/home']);
                        }else{
                            //Si no es correcta borra el usuario recogido y muestra mensaje de error
                            this.ErrorMessage = "El nombre de usuario o password no es correcto.";
                            this.UsuarioEntrante = null;
                        }
                    }
                },
                error => {
                    //Si el servidor tiene algún tipo de problema mostraremos este error
                    if(error==404){
                        this.ErrorMessage = "El nombre de usuario o password no es correcto."
                    }else if(error==500){
                        this.ErrorMessage = "Ocurrio un error en el servidor, contacte con el servicio técnico."
                    }
                }	
            );
        }else{
            this.ErrorMessage="Introduzca todos los campos porfavor."
        }
    }

    /*public Prueba(){
        this._userService.getUsers().subscribe(
            res => {
                if(!res){
                    console.log("No hay usuarios")
                }else{
                    console.log("Usuarios encontrados: "+res);
                }
            },
            error => {
                console.log("Lo sentimos ocurrio un error en al realizar la petición.");

            }	
        );
    }*/

}
