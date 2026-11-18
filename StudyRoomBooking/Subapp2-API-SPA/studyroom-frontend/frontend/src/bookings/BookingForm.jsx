import { useState } from 'react';
import { Form, Button, Alert } from 'react-bootstrap';

// Felles skjema for oppretting og redigering av bookinger.
const BookingForm = ({
    onBookingChanged,
    onCancel,
    rooms = [],
    initialValues = {},
    bookingId,
    submitting = false
}) => {

    const [roomId, setRoomId] = useState(initialValues.RoomId ?? '');
    const [startTime, setStartTime] = useState(initialValues.StartTime ?? '');
    const [endTime, setEndTime] = useState(initialValues.EndTime ?? '');
    const [course, setCourse] = useState(initialValues.Course ?? '');
    const [description, setDescription] = useState(initialValues.Description ?? '');
    const [validationError, setValidationError] = useState('');
    const isEditing = bookingId !== undefined && bookingId !== null;

    // Sender bookinginformasjonen til foreldresiden.
    const handleSubmit = async (event) => {
        event.preventDefault();

        setValidationError('');

        // Kontrollerer at brukeren har valgt et gyldig rom.
        if (!rooms.some(room => room.RoomId === Number(roomId))) {
            setValidationError('Du må velge et gyldig rom.');
            return;
        }

        // Kontrollerer at starttid og sluttid er fylt ut.
        if (!startTime || !endTime) {
            setValidationError('Du må velge starttid og sluttid.');
            return;
        }

        // Gjør tidspunktene om til datoer som kan sammenlignes.
        const start = new Date(startTime);
        const end = new Date(endTime);

        // Kontrollerer at tidspunktene er gyldige.
        if (Number.isNaN(start.getTime()) || Number.isNaN(end.getTime())) {
            setValidationError('Du må oppgi gyldige tidspunkter.');
            return;
        }

        // Starttidspunktet kan ikke være i fortiden.
        if (start < new Date()) {
            setValidationError('Starttidspunktet kan ikke være i fortiden.');
            return;
        }

        // Sluttidspunktet må være etter starttidspunktet.
        if (end <= start) {
            setValidationError('Sluttidspunktet må være etter starttidspunktet.');
            return;
        }

        // Kontrollerer at beskrivelsen ikke er tom.
        if (!description.trim()) {
            setValidationError('Du må skrive inn en beskrivelse.');
            return;
        }
        // Kurs er valgfritt, men kan ikke overstige 30 tegn.
        if (course.trim().length > 30) {
            setValidationError('Kursnavn kan ikke være lengre enn 30 tegn.');
            return;
        }

        // Beskrivelsen kan ikke overstige 200 tegn.
        if (description.trim().length > 200) {
            setValidationError('Beskrivelsen kan ikke være lengre enn 200 tegn.');
            return;
        }
        const booking = {
            Id: bookingId,
            RoomId: Number(roomId),
            StartTime: startTime,
            EndTime: endTime,
            Course: course.trim(),
            Description: description.trim()
        };

        try {
            await onBookingChanged(booking);
        } catch (error) {
            setValidationError(error.message);
        }
    };

    return (
        <Form className="booking-form" noValidate onSubmit={handleSubmit}>
            <Form.Group className="mb-3" controlId="bookingRoom">
                <Form.Label>Rom</Form.Label>
                <Form.Select
                    value={roomId}
                    onChange={e => setRoomId(e.target.value)}
                    required
                >
                    <option value="">Velg rom</option>
                    {rooms.map(room => (
                        <option key={room.RoomId} value={room.RoomId}>
                            {room.RoomName}
                        </option>
                    ))}
                </Form.Select>
            </Form.Group>

            <Form.Group className="mb-3" controlId="bookingStart">
                <Form.Label>Starttid</Form.Label>
                <Form.Control
                    type="datetime-local"
                    value={startTime}
                    onChange={e => setStartTime(e.target.value)}
                    required
                />
            </Form.Group>

            <Form.Group className="mb-3" controlId="bookingEnd">
                <Form.Label>Sluttid</Form.Label>
                <Form.Control
                    type="datetime-local"
                    value={endTime}
                    onChange={e => setEndTime(e.target.value)}
                    required
                />
            </Form.Group>

            <Form.Group className="mb-3" controlId="bookingCourse">
                <Form.Label>Kurs (valgfritt)</Form.Label>
                <Form.Control
                    type="text"
                    value={course}
                    onChange={e => setCourse(e.target.value)}
                />
            </Form.Group>

            <Form.Group className="mb-3" controlId="bookingDescription">
                <Form.Label>Beskrivelse</Form.Label>
                <Form.Control
                    as="textarea"
                    rows={3}
                    value={description}
                    onChange={e => setDescription(e.target.value)}
                    required
                />
            </Form.Group>


            {/* Viser feilmelding dersom informasjonen er ugyldig. */}
            {validationError && (
                <Alert variant="danger" className="fw-bold">
                    {validationError}
                </Alert>
            )}

            <div className="booking-form-actions">
                <Button
                    type="submit"
                    variant="primary"
                    className="room-search-button"
                    disabled={submitting}
                >
                    {submitting
                        ? 'Lagrer …'
                        : isEditing
                            ? 'Lagre endringer'
                            : 'Opprett booking'}
                </Button>

                <Button
                    type="button"
                    variant="outline-dark"
                    onClick={onCancel}
                    disabled={submitting}
                >
                    Avbryt
                </Button>
            </div>
        </Form>
    );
};

export default BookingForm;
