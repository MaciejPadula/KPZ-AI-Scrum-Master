import {
  ChangeDetectionStrategy,
  Component,
  effect,
  inject,
  OnInit,
  signal,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserSettingsDataService } from './services/user-settings-data.service';
import { UserSettings } from './models/user-settings';
import { TaigaAuthorizationService } from './services/taiga-authorization.service';
import { TaigaAuthorizationComponent } from './components/taiga-authorization/taiga-authorization.component';

@Component({
  selector: 'app-user-settings',
  standalone: true,
  templateUrl: './user-settings.component.html',
  imports: [CommonModule, TaigaAuthorizationComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UserSettingsComponent implements OnInit {
  private readonly userSettingsDataService = inject(UserSettingsDataService);
  private readonly taigaAuthorizationService = inject(
    TaigaAuthorizationService
  );

  #settings = signal<UserSettings | null>(null);
  public settings = this.#settings.asReadonly();

  public ngOnInit(): void {
    this.userSettingsDataService.getUserSettings().subscribe((settings) => {
      this.#settings.set(settings);
      this.taigaAuthorizationService.loggedToTaiga = settings.isLoggedToTaiga;
    });
  }
}
