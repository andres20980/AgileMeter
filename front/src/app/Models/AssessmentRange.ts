export class AssessmentRange{
    maxRange: number;
    rangeColors = ["#10ad9f","#1b5e20","#388e3c","#87a900"];
    
    constructor(private max: number)
    {
        this.maxRange = max
    }

    get range()
    {
        return Array(this.maxRange).fill(1).map((x,i) => (x * i) + 1);
    }
}