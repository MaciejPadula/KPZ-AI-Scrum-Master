import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { TaskRow } from '../../../shared/components/task-row/task-row';
import { map, Observable } from 'rxjs';
import { GetStoryTasksResponse } from '../models/get-story-tasks-response';

@Injectable({
  providedIn: 'root',
})
export class StoryTasksService {
  private readonly httpClient = inject(HttpClient);

  private readonly baseApiUrl = 'api/projects/userStories';

  public getStoryTasks(storyId: number): Observable<TaskRow[]> {
    return this.httpClient
      .get<GetStoryTasksResponse>(`${this.baseApiUrl}/${storyId}/tasks`)
      .pipe(map((response) => response.tasks));
  }
}
