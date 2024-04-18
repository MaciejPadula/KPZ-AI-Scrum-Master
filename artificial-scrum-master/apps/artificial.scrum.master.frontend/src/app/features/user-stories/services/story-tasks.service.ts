import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { TaskRow } from '../../../shared/components/task-row/task-row';
import { map, Observable, of } from 'rxjs';
import { GetStoryTasksResponse } from '../models/get-story-tasks-response';

@Injectable({
  providedIn: 'root',
})
export class StoryTasksService {
  private readonly httpClient = inject(HttpClient);

  private readonly baseApiUrl = 'api/projects/userStories/tasks';

  public getStoryTasks(
    storyId: number | null,
    sprintId: number | null
  ): Observable<TaskRow[]> {
    if (storyId === null && sprintId === null) {
      return of([]);
    }

    let params = new HttpParams();
    if (storyId !== null) {
      params = params.set('userStoryId', storyId.toString());
    } else if (sprintId !== null) {
      params = params.set('sprintId', sprintId.toString());
    }

    return this.httpClient
      .get<GetStoryTasksResponse>(this.baseApiUrl, { params })
      .pipe(map((response) => response.tasks));
  }
}
