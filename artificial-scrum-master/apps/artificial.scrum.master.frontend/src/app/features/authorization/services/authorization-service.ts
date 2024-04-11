import { SocialAuthService, SocialUser } from "@abacritt/angularx-social-login";
import { HttpClient, } from "@angular/common/http";
import { inject, Injectable, signal } from "@angular/core";
import { UserData } from "../models/user-data";

@Injectable({
    providedIn: 'root'
})

export class AuthorizationService {
    #userData = signal<UserData>(
        {
            isAuthorized: false,
            userId: null,
            userName: null,
            userPhotoUrl: null
        }
    );
    public userData = this.#userData.asReadonly();

    private googleAuthService = inject(SocialAuthService);
    private httpClient = inject(HttpClient);

    constructor() {
        this.getServerAuthorizationStatus();

        this.googleAuthService.authState.subscribe((user: SocialUser) => {
            const headers = { "Authorization": `Bearer ${user.idToken}` };
            this.httpClient.post("/api/user/google-sign-in", {}, { headers })
                .subscribe(
                    {
                        next: () => {
                            this.#userData.set({
                                isAuthorized: true,
                                userId: user.id,
                                userName: user.name,
                                userPhotoUrl: user.photoUrl
                            });
                        },
                        error: () => {
                            this.#userData.set({
                                isAuthorized: false,
                                userId: null,
                                userName: null,
                                userPhotoUrl: null
                            });
                        }
                    });
        });
    }

    logout() {
        this.httpClient.post("/api/user/logout", {}).subscribe(
            {
                next: () => {
                    this.#userData.set({
                        isAuthorized: false,
                        userId: null,
                        userName: null,
                        userPhotoUrl: null
                    });
                }
            });

    }


    private getServerAuthorizationStatus() {
        this.httpClient.get<UserData>("/api/user/user-info").subscribe(response => {
            this.#userData.set(response);
        });
    }
}