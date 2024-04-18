import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, map } from 'rxjs';
import { Session } from '../../../shared/models/session';
import { GetProjectSessionsResponse } from '../models/get-project-sessions-response';

@Injectable({
  providedIn: 'root',
})
export class ProjectPokerDataService {
  private readonly httpClient = inject(HttpClient);

  private readonly baseApiUrl = 'api/sessions';

  public createSession(
    sessionName: string,
    projectId: number
  ): Observable<void> {
    return this.httpClient.post<void>(`${this.baseApiUrl}`, {
      name: sessionName,
      projectId: projectId,
    });
  }

  public getProjectSessions(projectId: number): Observable<Session[]> {
    return this.httpClient
      .get<GetProjectSessionsResponse>(`${this.baseApiUrl}/${projectId}`)
      .pipe(map((response) => response.sessions));
  }
}
