import { ChangeDetectionStrategy, Component, Input, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../../../../shared/material.module';
import { TaigaAccess } from '../../models/taiga-access';

@Component({
  selector: 'app-taiga-authorization',
  standalone: true,
  imports: [CommonModule, MaterialModule],
  templateUrl: './taiga-authorization.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TaigaAuthorizationComponent {
  #showLoginForm = signal(false);
  public showLoginForm = this.#showLoginForm.asReadonly();

  @Input() public taigaAccess: TaigaAccess | undefined = undefined;

  public showForm() {
    this.#showLoginForm.set(true);
  }

  public hideForm() {
    this.#showLoginForm.set(false);
  }
}
