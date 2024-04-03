import { Injectable } from '@angular/core';
import { GetUserProjectsResponse } from '../models/get-user-projects-response';
import { GetUserProjectsResponseElement } from '../models/user-project';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ProjectListDataService {
  public getProjectsEvents(): Observable<GetUserProjectsResponseElement[]> {
    return of(this.rows.projects);
  }

  private rows: GetUserProjectsResponse = {
    projects: [
      {
        id: 123,
        name: 'Project 1',
        ownerUsername: 'Owner 1',
        isPrivate: false,
        amOwner: false,
        modifiedDate: new Date(),
      },
      {
        id: 456,
        name: 'Project 2',
        ownerUsername: 'Owner 2',
        isPrivate: true,
        amOwner: true,
        modifiedDate: new Date(),
      },
      {
        id: 789,
        name: 'Project 3',
        ownerUsername: 'Owner 3',
        isPrivate: true,
        amOwner: true,
        modifiedDate: new Date(),
      },
    ],
  };
}
