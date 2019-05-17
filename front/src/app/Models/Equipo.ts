import { Linea } from "./Linea";
import { Office } from "./Office";
import { Unity } from "./Unit";
import { Evaluacion } from "./Evaluacion";

export class Equipo{
	constructor(
		public id: number, 
        public nombre: string,
		public fecha: Date,		
		public projectSize: number,
		public testProject: boolean,
		public userNombre: string,
		public lineaEntity:Linea,
		public oficinaEntity: Office,
		public unidadEntity: Unity,
		public evaluaciones: Array<Evaluacion> = []
	){}
}
