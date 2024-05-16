import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { GetSessionCardsResponse } from '../models/get-session-cards-response';
import { CardType } from '../components/add-card-dialog/add-card-dialog-data';

@Injectable({
  providedIn: 'root',
})
export class RetrospectiveDataService {
  private readonly httpClient = inject(HttpClient);

  private readonly baseApiUrl = 'api/retrospectives';

  public getSessionCards(
    sessionId: string
  ): Observable<GetSessionCardsResponse> {
    return this.httpClient.get<GetSessionCardsResponse>(
      `${this.baseApiUrl}/cards/${sessionId}`
    );
  }

  public createSessionCard(content: string, sessionId: string, type: CardType) {
    return this.httpClient.post(`${this.baseApiUrl}/cards`, {
      content: content,
      sessionId: sessionId,
      type: type,
    });
  }
}