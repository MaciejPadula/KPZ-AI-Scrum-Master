import { inject, Injectable, signal } from '@angular/core';
import { UserSettingsDataService } from './user-settings-data.service';
import { HttpClient } from '@angular/common/http';
import { map, Observable, switchMap } from 'rxjs';
import { TaigaAuthResponse } from '../models/taiga-auth-response';
import { TaigaAccess } from '../models/taiga-access';

@Injectable({
  providedIn: 'root',
})
export class TaigaAuthorizationService {
  private readonly httpClient = inject(HttpClient);
  private readonly userSettingsDataService = inject(UserSettingsDataService);

  #isLoggedToTaiga = signal(false);
  public isLoggedToTaiga = this.#isLoggedToTaiga.asReadonly();

  private readonly loginType: string = 'normal';

  public loginToTaiga(login: string, password: string): Observable<void> {
    return this.httpClient
      .post<TaigaAuthResponse>('https://api.taiga.io/api/v1/auth', {
        username: login,
        password,
        type: this.loginType,
      })
      .pipe(
        switchMap((response) =>
          this.userSettingsDataService.setTaigaAccess(<TaigaAccess>{
            accessToken: response.auth_token,
            refreshToken: response.refresh,
          })
        ),
        map(() => this.#isLoggedToTaiga.set(true))
      );
  }

  public set loggedToTaiga(value: boolean) {
    this.#isLoggedToTaiga.set(value);
  }
}
