import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../../material.module';
import { MatIconRegistry } from '@angular/material/icon';
import { DomSanitizer } from '@angular/platform-browser';

const TAIGA_ICON = `
<?xml version="1.0" encoding="UTF-8" standalone="no"?>
<svg width="256px" height="256px" viewBox="0 0 256 256" version="1.1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" preserveAspectRatio="xMidYMid">
	<g>
		<path d="M224.287568,43.9155813 L212.324664,128.115466 L128.124779,116.152563 L140.087683,31.9526775 L224.287568,43.9155813 L224.287568,43.9155813 Z" opacity="0.8" fill="#A295AE"></path>
		<path d="M31.8853252,212.290983 L43.848229,128.091098 L128.048114,140.054002 L116.08521,224.253887 L31.8853252,212.290983 L31.8853252,212.290983 Z" opacity="0.8" fill="#5D6F6D"></path>
		<path d="M43.8482612,32.0645356 L128.048146,44.0274395 L116.085243,128.227325 L31.8853574,116.264421 L43.8482612,32.0645356 L43.8482612,32.0645356 Z" opacity="0.8" fill="#8CD592"></path>
		<path d="M212.226084,224.263692 L128.026199,212.300788 L139.989103,128.100903 L224.188988,140.063807 L212.226084,224.263692 L212.226084,224.263692 Z" opacity="0.8" fill="#665E74"></path>
		<path d="M119.642193,255.595097 L68.562934,187.597737 L136.560294,136.518478 L187.639553,204.515838 L119.642193,255.595097 L119.642193,255.595097 Z" opacity="0.8" fill="#3C3647"></path>
		<path d="M255.463211,136.38964 L187.465851,187.4689 L136.386592,119.47154 L204.383953,68.3922807 L255.463211,136.38964 L255.463211,136.38964 Z" opacity="0.8" fill="#837193"></path>
		<path d="M136.436624,0.553850534 L187.515883,68.5512107 L119.518523,119.63047 L68.4392642,51.6331097 L136.436624,0.553850534 L136.436624,0.553850534 Z" opacity="0.8" fill="#A2F4AC"></path>
		<path d="M0.463311092,119.700163 L68.4606712,68.6209042 L119.53993,136.618264 L51.5425699,187.697523 L0.463311092,119.700163 L0.463311092,119.700163 Z" opacity="0.8" fill="#7EA685"></path>
		<path d="M127.963225,95.7423436 L160.2954,128.074519 L127.963225,160.406694 L95.6310499,128.074519 L127.963225,95.7423436 L127.963225,95.7423436 Z" fill="#3C3647"></path>
	</g>
</svg>`;

@Component({
  selector: 'app-taiga-icon',
  standalone: true,
  imports: [CommonModule, MaterialModule],
  templateUrl: './taiga-icon.component.html',
})
export class TaigaIconComponent {
  constructor(iconRegistry: MatIconRegistry, sanitizer: DomSanitizer) {
    iconRegistry.addSvgIconLiteral('taiga-icon', sanitizer.bypassSecurityTrustHtml(TAIGA_ICON));
  }
}
