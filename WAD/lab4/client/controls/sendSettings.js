export default function send(url, method, data = '') {
    let req = {
        method: method,
        headers: {
            "Content-type": "application/json"
        }
    };

    if (data !== '') {
        req.body = JSON.stringify(data);
    }

    return fetch(url, req);
}