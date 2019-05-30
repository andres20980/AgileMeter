import { Role } from "./Role";

export class UserCreateUpdate {
	constructor(
		public nombre: string,
		public password: string,
		public nombreCompleto: string,
		public role: Role
	) {
	}
}