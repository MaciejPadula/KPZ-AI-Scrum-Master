import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MenuComponent } from "./features/menu/components/menu.component";

@Component({
    standalone: true,
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrl: './app.component.scss',
    imports: [
        RouterModule,
        MenuComponent
    ]
})
export class AppComponent {
  title = 'artificial.scrum.master.frontend';
}
