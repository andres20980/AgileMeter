
export class EvaluacionInfoWithProgress {
  constructor(
    public id: number,
    public nombre: string,
    public userNombre: string,
    public fecha: string,
    public assessmentId: number,
    public assessmentName: string,
    public progress: number,
    public numQuestions: number,
    public oficina: string,
    public assessmentRange: number
  ) { }
}
