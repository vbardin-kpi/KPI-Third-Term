'use strict';
import { ButtonControls } from "./controls/ButtonControls.js";
import request from "./controls/sendSettings.js";

document.addEventListener('DOMContentLoaded', async () => {
    const settings = JSON.parse(await (request('/settings', 'GET')).then(r => r.text()));
    const buttons = new ButtonControls(settings);
    buttons.initButtons();
    await request('/settings', settings);
    await request('/storage', localStorage);
});