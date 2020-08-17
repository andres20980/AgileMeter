export class AssessmentColumns {
    constructor(){}


    public get displayedScrumColumns(): Array<any>{ 
        return  ['fecha', 'userNombre', 'oficina', 'nombre', 'assessmentName','equipo','eventos','herramientas','mindset','aplicacion','puntuacion', 'notas', 'informe'];
    }


    public get displayedDevopsColumns(): Array<any>{ 
    return ['fecha', 'userNombre', 'oficina', 'nombre', 'assessmentName','orgequipo','ciclovida','construccion','testing','despliegue','monitorizacion','aprovisionamiento','puntuacion', 'notas', 'informe'];
    }

    public get displayedKanbanColumns(): Array<any>{
        return  ['fecha', 'userNombre', 'oficina', 'nombre', 'assessmentName','kbnequipo','kbnkanbanboard','kbnpracticas','kbnmindset','kbnaplicacion','puntuacion', 'notas', 'informe'];
     }

     
    public get displayedRemoteColumns(): Array<any>{
        return  ['fecha', 'userNombre', 'oficina', 'nombre', 'assessmentName','rmtespacio','rmtcomunicacion','rmtliderazgo','rmtcorporativo','puntuacion', 'notas', 'informe'];
     }



    public get excelScrum(): Array<any>{
        return [
            ["equipo", "EXCEL_PT_SCRUM.TEAM", 20,"SCRUM", "String"],
            ["eventos", "EXCEL_PT_SCRUM.EVENTS",20,"SCRUM", "String"], 
            ["herramientas", "EXCEL_PT_SCRUM.TOOLS",20,"SCRUM", "String"],
            ["mindset", "EXCEL_PT_SCRUM.MINDSET", 20,"SCRUM", "String"],
            ["aplicacion", "EXCEL_PT_SCRUM.APP",20,"SCRUM", "String"]];
    }


    public get excelDevops(): Array<any>
    {
        return [
            ["orgequipo", "EXCEL_PT_DEVOPS.ORG_TEAM", 20,"DEVOPS", "String"],
            ["ciclovida", "EXCEL_PT_DEVOPS.LIFECYCLE", 20,"DEVOPS", "String"],
            ["construccion", "EXCEL_PT_DEVOPS.BUILDING", 20,"DEVOPS", "String"],
            ["testing", "EXCEL_PT_DEVOPS.TESTING", 20,"DEVOPS", "String"],
            ["despliegue", "EXCEL_PT_DEVOPS.DEPLOYMENT", 20,"DEVOPS", "String"],
            ["monitorizacion", "EXCEL_PT_DEVOPS.MONITORING", 20,"DEVOPS", "String"],
            ["aprovisionamiento", "EXCEL_PT_DEVOPS.PROVISIONING", 20,"DEVOPS", "String"]];
    }

    public get execelKanban(): Array<any>
    {
        return  [
            ["kbnequipo", "EXCEL_PT_KANBAN.TEAM", 20,"KANBAN", "String"],
            ["kbnkanbanboard", "EXCEL_PT_KANBAN.BOARD", 20,"KANBAN", "String"],
            ["kbnpracticas", "EXCEL_PT_KANBAN.PRACTICES", 20,"KANBAN", "String"],
            ["kbnmindset", "EXCEL_PT_KANBAN.MINDSET", 20,"KANBAN", "String"],
            ["kbnaplicacion", "EXCEL_PT_KANBAN.PRACT_APL", 20,"KANBAN", "String"]];
    }

    public get execelRemote(): Array<any>
    {
        return  [
            ["rmtespacio", "EXCEL_PT_REMOTE.SPACEWORK", 20,"REMOTE", "String"],
            ["rmtcomunicacion", "EXCEL_PT_REMOTE.COMUNICATION", 20,"REMOTE", "String"],
            ["mtliderazgo", "EXCEL_PT_REMOTE.LEADING", 20,"REMOTE", "String"],
            ["rmtcorporativo", "EXCEL_PT_REMOTE.CORPORATIVE", 20,"REMOTE", "String"]];
    }
}