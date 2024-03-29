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
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { TaigaAuthFormControl } from './taiga-auth-form-control';

@Component({
  selector: 'app-taiga-authorization',
  standalone: true,
  templateUrl: './taiga-authorization.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule, MaterialModule, ReactiveFormsModule],
})
export class TaigaAuthorizationComponent {
  private readonly taigaAuthorizationService = inject(
    TaigaAuthorizationService
  );

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
  }

  public get isFormValid() {
    return this.formGroup.valid;
  }

  public submit() {
    const login = this.formGroup.get('login')?.value ?? '';
    const password = this.formGroup.get('password')?.value ?? '';

    this.taigaAuthorizationService.loginToTaiga(login, password);
  }
}
