import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
  signal,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../../../../shared/material.module';
import { TaigaAuthorizationService } from '../../services/taiga-authorization.service';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { TaigaAuthFormControl } from './taiga-auth-form-control';
import { ToastService } from '../../../../shared/services/toast.service';
import { TranslateModule, TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-taiga-authorization',
  standalone: true,
  templateUrl: './taiga-authorization.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule, MaterialModule, ReactiveFormsModule, TranslateModule],
})
export class TaigaAuthorizationComponent {
  private readonly taigaAuthorizationService = inject(
    TaigaAuthorizationService
  );
  private readonly toastService = inject(ToastService);
  private readonly translateService = inject(TranslateService);

  public formGroup = new FormGroup<TaigaAuthFormControl>({
    login: new FormControl<string | null>(null, [Validators.required]),
    password: new FormControl<string | null>(null, [Validators.required]),
  });

  #forceShowLoginForm = signal(false);

  public showLoginForm = computed(() => {
    return (
      this.#forceShowLoginForm() ||
      !this.taigaAuthorizationService.isLoggedToTaiga()
    );
  });

  public forceLoginForm() {
    this.#forceShowLoginForm.set(true);
  }

  public cancelLoginFormIfPossible() {
    this.#forceShowLoginForm.set(false);
    this.formGroup.reset();
  }

  public get isFormValid() {
    return this.formGroup.valid;
  }

  public submit() {
    const login = this.formGroup.get('login')?.value ?? '';
    const password = this.formGroup.get('password')?.value ?? '';

    this.taigaAuthorizationService.loginToTaiga(login, password).subscribe({
      next: () => {
        this.formGroup.reset();
        this.#forceShowLoginForm.set(false);
        this.toastService.openSuccess(
          this.translateService.instant('UserSettings.Tagia.LoginSuccess')
        );
      },
      error: () => {
        this.toastService.openError(
          this.translateService.instant('UserSettings.Tagia.LoginError')
        );
      },
    });
  }
}
