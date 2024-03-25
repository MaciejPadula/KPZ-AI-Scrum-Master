import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { UserSettings } from '../models/user-settings';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UserSettingsDataService {
  private httpClient = inject(HttpClient);

  private readonly BASE_API_URL = 'api/user-settings';

  public getUserSettings(): Observable<UserSettings> {
    return this.httpClient.get<UserSettings>(this.BASE_API_URL);
  }

  public updateUserSettings(userSettings: UserSettings): Observable<void> {
    return this.httpClient.post<void>(this.BASE_API_URL, userSettings);
  }
}
