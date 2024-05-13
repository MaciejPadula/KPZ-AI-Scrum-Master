import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GetStoryTaskSuggestion } from '../models/get-story-task-suggestion';

@Injectable({
  providedIn: 'root',
})
export class StoryTaskSuggestionService {
  private readonly httpClient = inject(HttpClient);
  private readonly baseApiUrl = 'api/task/suggestions';

  public getTaskDescriptionSuggestion(
    taskTitle: string,
    userStoryTitle: string | null,
    taskDescription: string | null
  ): Observable<GetStoryTaskSuggestion> {
    return this.httpClient.post<GetStoryTaskSuggestion>(`${this.baseApiUrl}`, {
      userStoryTitle: userStoryTitle,
      taskTitle: taskTitle,
      taskDescription: taskDescription,
    });
  }
}
