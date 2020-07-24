
export class EvaluacionInfo {
  constructor(
    public id: number,
    public nombre: string,
    public userNombre: string,
    public puntuacion: number,
    public media: number,
    public fecha: string,
    public notasEvaluacion: string,
    public notasObjetivos: string,
    public estado: boolean,
    public flagNotasSec: boolean,
    public flagNotasAsig: boolean,
    public assessmentId: number,
    public assessmentName: string,
    public AssessmentRange: number,
    public oficina: string,
    public proyectoId: number
  ) { }
}
