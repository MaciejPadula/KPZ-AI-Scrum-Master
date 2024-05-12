import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateSessionIfNotExistsResponse } from '../models/create-session-if-not-exists-response';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class RetroDataService {
  private readonly httpClient = inject(HttpClient);

  private readonly baseApiUrl = 'api/retrospectives';

  public createSessionIfNotExists(
    sprintId: number,
    sprintName: string,
    projectId: number
  ): Observable<CreateSessionIfNotExistsResponse> {
    return this.httpClient.post<CreateSessionIfNotExistsResponse>(
      `${this.baseApiUrl}/create-if-not-exists`,
      {
        sprintId: sprintId,
        name: sprintName,
        projectId: projectId,
      }
    );
  }
}
