const express = require("express");
const fs = require("fs/promises");
const app = express();

const jsonParser = express.json();
app.use('/', express.static('./client'));

app.get('/settings', async (req, res) => {
    const data  = (await fs.readFile('settings.json')).toString();
    res.send(data);
});

app.post('/settings', jsonParser, async (req, res) => {
    await fs.writeFile('settings.json', JSON.stringify(req.body));
    res.send('201');
});

const port = process.env.PORT || 3000;
app.listen(port);