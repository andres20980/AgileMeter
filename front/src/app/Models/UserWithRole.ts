import { Role } from 'app/Models/Role';

export class UserWithRole{
	constructor(
		public nombre: string, 
		public  nombreCompleto: string,
        public  activo: boolean,
        public role: Role
		){}
}