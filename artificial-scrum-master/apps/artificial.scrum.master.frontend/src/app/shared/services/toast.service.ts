import { inject, Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class ToastService {
  private readonly snackBar = inject(MatSnackBar);

  private readonly toastDurationInMs = 2000;

  public openSuccess(message: string): void {
    this.snackBar.open(message, 'Zamknij', {
      duration: this.toastDurationInMs,
    });
  }

  public openError(message: string): void {
    this.snackBar.open(message, 'Zamknij', {
      duration: this.toastDurationInMs,
    });
  }
}
