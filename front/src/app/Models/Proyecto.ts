export class Proyecto {
	constructor(
		public id: number,
		public nombre: string,
		public codigo: string,
		public fecha: Date,
		public numFinishedEvals: number,
		public numPendingEvals: number,
		public oficina: string,
		public proyecto: string,
	) { }
}
