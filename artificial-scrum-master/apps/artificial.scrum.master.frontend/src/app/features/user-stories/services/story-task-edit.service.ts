import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TaskDetails } from '../../../shared/components/task-details/task-details';

@Injectable({
  providedIn: 'root',
})
export class StoryTaskEditService {
  private readonly httpClient = inject(HttpClient);
  private readonly baseApiUrl = 'api/task';

  public patchTaskDescription(
    taskId: number,
    version: number,
    description: string
  ): Observable<TaskDetails> {
    return this.httpClient.patch<TaskDetails>(`${this.baseApiUrl}/${taskId}`, {
      version,
      description,
    });
  }
}
