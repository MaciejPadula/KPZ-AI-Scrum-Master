import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { UserSettings } from '../models/user-settings';
import { Observable } from 'rxjs';
import { TaigaAccess } from '../models/taiga-access';

@Injectable({
  providedIn: 'root',
})
export class UserSettingsDataService {
  private readonly httpClient = inject(HttpClient);

  private readonly baseApiUrl = 'api/user-settings';

  public getUserSettings(): Observable<UserSettings> {
    return this.httpClient.get<UserSettings>(this.baseApiUrl);
  }

  public setTaigaAccess(taigaAccess: TaigaAccess): Observable<void> {
    return this.httpClient.post<void>(`${this.baseApiUrl}/set-taiga-access`, taigaAccess);
  }
}
