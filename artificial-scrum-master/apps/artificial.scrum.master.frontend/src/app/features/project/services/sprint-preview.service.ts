import { inject, Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { GetSprintPreviewResponse } from '../models/get-sprint-preview-response';
import { SprintPreview } from '../models/sprint-preview';

@Injectable({
  providedIn: 'root',
})
export class SprintPreviewDataService {
  private readonly httpClient = inject(HttpClient);

  private readonly baseApiUrl = 'api/projects/sprints';

  public getSprintPreviews(projectId: number): Observable<SprintPreview[]> {
    return this.httpClient
      .get<GetSprintPreviewResponse>(`${this.baseApiUrl}/${projectId}`)
      .pipe(map((response) => response.sprints));
  }
}
