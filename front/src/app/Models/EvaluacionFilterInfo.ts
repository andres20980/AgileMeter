export class EvaluacionFilterInfo {
  constructor(
    public nombre: string,
    public userNombre: string,
    public puntuacion: string,
    public fecha: string,
    public estado: string,
    public assessmentId: number,    
    public oficinas:string[],
    public equipos:number[],
    public idAssessment:number[],
  ) { }
}
