import { inject, Injectable, signal } from '@angular/core';
import { UserSettingsDataService } from './user-settings-data.service';
import { from, map, Observable, switchMap } from 'rxjs';
import { TaigaAuthResponse } from '../models/taiga-auth-response';
import { TaigaAccess } from '../models/taiga-access';

@Injectable({
  providedIn: 'root',
})
export class TaigaAuthorizationService {
  private readonly userSettingsDataService = inject(UserSettingsDataService);

  #isLoggedToTaiga = signal(false);
  public isLoggedToTaiga = this.#isLoggedToTaiga.asReadonly();

  private readonly loginType: string = 'normal';

  // HttpRequest copy messed with cors policy
  public loginToTaiga(login: string, password: string): Observable<void> {
    return from(
      fetch('https://api.taiga.io/api/v1/auth', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          username: login,
          password,
          type: this.loginType,
        }),
      })
    ).pipe(
      switchMap((x) => {
        if (x.ok) {
          return from(x.json());
        }

        throw new Error('Error while logging to Taiga');
      }),
      switchMap((response: TaigaAuthResponse) =>
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
