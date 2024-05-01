import { Injectable, signal } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class UsersListService {
  private readonly hubConnection: signalR.HubConnection = new signalR.HubConnectionBuilder()
    .withUrl('/hub/users-list')
    .build();

  #users = signal<string[]>([]);
  public users = this.#users.asReadonly();

  private sessionId: string;

  public async connectScrumMaster(sessionId: string) {
    this.sessionId = sessionId;

    await this.hubConnection.start();
    await this.hubConnection.invoke("JoinAdmins", sessionId);
    await this.getUsers();

    this.hubConnection.on('GetActiveUsers', () => this.getUsers());
  }

  public connectDeveloper(sessionId: string) {
    this.sessionId = sessionId;
    this.hubConnection.start();
  }

  public registerUser(userName: string) {
    this.hubConnection.invoke('RegisterUser', this.sessionId, userName);
  }

  private async getUsers() {
    const result: string[] = await this.hubConnection.invoke('GetActiveUsers', this.sessionId);
    this.#users.set(result);
  }
}
