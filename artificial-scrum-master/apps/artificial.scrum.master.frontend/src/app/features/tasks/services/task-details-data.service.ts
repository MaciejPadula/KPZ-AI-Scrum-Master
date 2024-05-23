import { inject, Injectable } from '@angular/core';
import { TaskDetails } from '../models/task-details';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class TaskDetailsDataService {
  private readonly httpClient = inject(HttpClient);
  private readonly baseApiUrl = '/api/task';

  public getTaskDetails(taskId: number): Observable<TaskDetails> {
    return this.httpClient.get<TaskDetails>(`${this.baseApiUrl}/${taskId}`);
  }
}
