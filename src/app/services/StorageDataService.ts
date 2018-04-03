
import { Injectable, Inject } from '@angular/core';
import { User } from '../login/User'; 

@Injectable()
export class StorageDataService{
    public UserProjects: any = [];
    public UserProjectSelected: any = [];
    public UserData: User;
    public DataUnfinished: boolean = false;
}