import { Respuesta } from "./Respuesta";

export class Evaluacion {
  constructor(
    public id: number,
    public fecha: string,
    public estado: boolean,
    public notasObjetivos: string,
    public notasEvaluacion: string,
    public puntuacion: number,
    public nombre: string,
    public assessmentName: string,
    public userNombre: string,
    public oficina: string,
    public assessmentId?: number,
    public sectionsInfo?: Array<any>,
    public proyectoId?: number
  ) { }
}
