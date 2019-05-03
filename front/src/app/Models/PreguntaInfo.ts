import { Respuesta } from "app/Models/Respuesta";

export class PreguntaInfo {
  constructor(
    public id: number,
    public pregunta: string,
    public respuesta: Respuesta,
    public correcta: string,
    public esHabilitante: boolean,
    public preguntaHabilitanteId: number

  ) { }
}

