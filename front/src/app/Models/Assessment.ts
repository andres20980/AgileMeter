export class Assessment{
    assessmentId:number;
    assessmentName: string;
    assessmentRange: number
    constructor(assessmentId: number,assessmentName:string, assessmentRange: number)
    {
        this.assessmentId = assessmentId;
        this.assessmentName = assessmentName;
        this.assessmentRange = assessmentRange;

    }
}