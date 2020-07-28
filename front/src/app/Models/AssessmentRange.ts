export class AssessmentRange{
    maxRange: number;
    rangeColors = ["#A8C651","#68C651","#4BB15A","#4BB18A"];
    
    constructor(private max: number)
    {
        this.maxRange = max
    }

    get range()
    {
        return Array(this.maxRange).fill(1).map((x,i) => (x * i) + 1);
    }
}