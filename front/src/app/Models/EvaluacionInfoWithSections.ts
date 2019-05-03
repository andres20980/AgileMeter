import { SectionInfo } from "./SectionInfo";

export class EvaluacionInfoWithSections {
  constructor(
    public id: number,
    public nombre: string,
    public userNombre: string,
    public puntuacion: number,
    public media: number,
    public fecha: string,
    public estado: boolean,
    public assessmentId: number,
    public assessmentName: string,
    public notasEvaluacion: string,
    public notasObjetivos: string,
    public sectionsInfo: Array<SectionInfo>

  ) { }
}
