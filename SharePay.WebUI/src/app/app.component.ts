import { Component } from '@angular/core';
import { RouterOutlet, Router } from '@angular/router';

import {ClarityModule} from '@clr/angular'
import '@clr/icons';
import '@clr/icons/shapes/all-shapes';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, ClarityModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {

  constructor(private router: Router) {
  }

  title = 'SharePay.WebUI';
}
