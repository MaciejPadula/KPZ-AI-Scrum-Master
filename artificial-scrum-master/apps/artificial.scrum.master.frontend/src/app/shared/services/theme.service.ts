import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ThemeService {
  #darkTheme = signal(false);
  public readonly darkTheme = this.#darkTheme.asReadonly();

  public setDarkTheme(): void {
    this.#darkTheme.set(true);
    document.body.classList.add('dark-theme');
  }

  public setLightTheme(): void {
    this.#darkTheme.set(false);
    document.body.classList.remove('dark-theme');
  }
}
