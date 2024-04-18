import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, map } from 'rxjs';
import { SessionTask } from '../models/session-task';
import { GetCurrentTaskResponse } from '../models/get-curren-task-response';
import { GetTaskEstimationsResponse } from '../models/get-task-estimations-response';

@Injectable({
  providedIn: 'root',
})
export class EstimationPokerDataService {
  private readonly httpClient = inject(HttpClient);

  private readonly baseApiUrl = 'api/sessions';

  public addTask(
    sessionId: string,
    title: string,
    desctiption: string
  ): Observable<void> {
    return this.httpClient.post<void>(`${this.baseApiUrl}/tasks`, {
      sessionId: sessionId,
      title: title,
      description: desctiption,
    });
  }

  public addTaskEstimation(
    taskId: number,
    username: string,
    sessionId: string,
    estimation: number
  ): Observable<void> {
    return this.httpClient.post<void>(`${this.baseApiUrl}/estimations`, {
      estimationValue: estimation,
      taskId: taskId,
      username: username,
      sessionId: sessionId,
    });
  }

  public getSessionTask(sessionId: string): Observable<SessionTask | null> {
    return this.httpClient
      .get<GetCurrentTaskResponse>(`${this.baseApiUrl}/tasks/${sessionId}`)
      .pipe(map((response) => response.currentTask));
  }

  public getSessionTaskEstimations(taskId: number): Observable<GetTaskEstimationsResponse> {
    return this.httpClient.get<GetTaskEstimationsResponse>(`${this.baseApiUrl}/estimations/${taskId}`);
  }
}
