import http from 'k6/http';
import { check, sleep } from 'k6';
import { parseHTML } from 'k6/html';

export const options = {
    stages: [
        { duration: '5s', target: 20 },
        { duration: '10s', target: 20 },
        { duration: '5s', target: 0 },
    ],
    thresholds: {
        http_req_duration: ['p(95)<500'],
    },
};

export default function () {
    // 1. Read the URL from Environment Variable (or default to localhost for safety)
    // This is the k6 equivalent of Environment.GetEnvironmentVariable("AppUrl")
    const BASE_URL = __ENV.APP_URL || 'http://localhost:5000';

    // --- STEP 1: GET the page ---
    const resGet = http.get(BASE_URL);

    check(resGet, {
        'Homepage loaded': (r) => r.status === 200,
    });

    const doc = parseHTML(resGet.body);
    const verificationToken = doc.find('input[name="__RequestVerificationToken"]').val();

    if (!verificationToken) {
        console.error(`Could not find Token at ${BASE_URL}. Check URL or app deployment.`);
        return;
    }

    // --- STEP 2: POST the data ---
    const formData = {
        'BP.Systolic': '120',
        'BP.Diastolic': '80',
        '__RequestVerificationToken': verificationToken,
    };

    const resPost = http.post(BASE_URL, formData);

    check(resPost, {
        'Form submitted': (r) => r.status === 200,
        'Result displayed': (r) => r.body.includes('BP Category:'),
    });

    sleep(1);
}