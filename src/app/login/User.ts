import { Proyecto } from "./Proyecto";

export class User{
	constructor(
		public nombre: string, 
        public password: string,
        public proyectosDeUsuario:Array<Proyecto>,
		){}
}