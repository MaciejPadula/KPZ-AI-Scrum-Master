import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AddTaskDialogService {
  #loader = signal<boolean>(false);
  public loader = this.#loader.asReadonly();

  public startLoading() {
    this.#loader.set(true);
  }

  public stopLoading() {
    this.#loader.set(false);
  }
}
