import { inject, Injectable } from '@angular/core';
import { GetUserProjectsResponse } from '../models/get-user-projects-response';
import { GetUserProjectsResponseElement } from '../models/user-project';
import { Observable, map } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class ProjectListDataService {
  private readonly httpClient = inject(HttpClient);

  private readonly baseApiUrl = 'api/projects';

  public getProjectsEvents(): Observable<GetUserProjectsResponseElement[]> {
    return this.httpClient
      .get<GetUserProjectsResponse>(`${this.baseApiUrl}`)
      .pipe(map((response) => response.projects));
  }
}
