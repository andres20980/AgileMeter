export class EvaluacionCreate {
  constructor(
    public estado: boolean,
    public proyectoid: number,
    public userNombre: string,
    public assessmentId : number,
    public assesmentName: string
  ) { }
}
